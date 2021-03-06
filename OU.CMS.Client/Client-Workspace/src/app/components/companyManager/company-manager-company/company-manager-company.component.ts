import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { ManagerCompanyService } from 'src/app/shared/api/manager/managerCompany.service';
import {
  CompanyManagerDto,
  CreateCompanyManagementInviteDto,
  DeleteCompanyManagementDto,
  GetCompanyDto,
  GetCompanyManagementDto,
  RevokeCompanyManagementInviteDto,
  EditCompanyDto,
} from 'src/app/shared/models/CompanyModels';
import {
  faEnvelope,
  faPlusSquare,
  faTrashAlt,
  faUser,
  faUserShield,
} from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { NavbarService } from 'src/app/shared/services/navbar.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';

@Component({
  selector: 'app-company-manager-company',
  templateUrl: './company-manager-company.component.html',
  styleUrls: ['./company-manager-company.component.css'],
})
export class CompanyManagerCompanyComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private companyService: ManagerCompanyService,
    private navbarService: NavbarService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private modalService: NgbModal,
    private router: Router
  ) {}

  companyDetails: GetCompanyDto = new GetCompanyDto;
  companyManagers: GetCompanyManagementDto = new GetCompanyManagementDto;

  currentUserEmail: string;

  //icons
  adminIcon = faUserShield;
  userIcon = faUser;
  deleteIcon = faTrashAlt;
  addIcon = faPlusSquare;
  envelopeIcon = faEnvelope;

  addManagerForm: CreateCompanyManagementInviteDto;
  addManagerModal: NgbModalRef;

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Company);

    this.currentUserEmail = this.accountService.userInfo.Email;
    this.addManagerForm = new CreateCompanyManagementInviteDto();
    this.addManagerForm.CompanyId = this.accountService.userInfo.CompanyId;

    this.companyService
      .getCompany(this.accountService.userInfo.CompanyId)
      .subscribe((data) => {
        this.companyDetails = data;
      });

    this.fetchCompanyManagers();
  }

  // Company
  editCompany(form: EditCompanyDto) {
    this.companyService.editCompany(form).subscribe(
      (result) => {
        this.toastr.success(
          'The company was saved successfully.',
          'Company Updated!'
        );
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.message,
          'There was an error saving the company!'
        );
      }
    );
  }

  // Company Manager
  fetchCompanyManagers() {
    this.companyService
    .getCompanyManagement(this.accountService.userInfo.CompanyId)
    .subscribe((data) => {
      this.companyManagers = data;
    });
  }

  openAddManagerModal(content) {
    this.addManagerForm.CompanyId = this.accountService.userInfo.CompanyId;

    this.addManagerModal = this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  addManager(form: CreateCompanyManagementInviteDto) {
    this.companyService.createCompanyManagementInvite(form).subscribe(
      (result) => {
        this.toastr.success(
          'The manager was invited successfully.',
          'Manager Invited!'
        );

        this.fetchCompanyManagers();

        this.cd.detectChanges();
        this.addManagerModal.close();
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.message,
          'There was an error inviting the company manager!'
        );
      }
    );
  }

  removeManager(manager: CompanyManagerDto) {
    if (manager.HasAcceptedInvite) {
      let companyManager: DeleteCompanyManagementDto = {
        CompanyId: this.accountService.userInfo.CompanyId,
        UserId: manager.User.UserId,
      };

      this.companyService.deleteCompanyManagement(companyManager).subscribe(
        (result) => {
          this.toastr.success(
            'The manager was removed successfully.',
            'Manager Deleted!'
          );
        },
        (error) => {
          console.error(error);
          this.toastr.error(
            error.message,
            'There was an error removing the company manager!'
          );
        }
      );
    } else {
      let companyManagerInvite: RevokeCompanyManagementInviteDto = {
        CompanyId: this.accountService.userInfo.CompanyId,
        Email: manager.InviteeEmail,
      };

      this.companyService
        .revokeCompanyManagementInvite(companyManagerInvite)
        .subscribe(
          (result) => {
            this.toastr.success(
              'The manager invite was removed successfully.',
              'Manager Invite Deleted!'
            );
          },
          (error) => {
            console.error(error);
            this.toastr.error(
              error.message,
              'There was an error removing the company manager invite!'
            );
          }
        );
    }

    this.companyManagers.CompanyManagers = this.companyManagers.CompanyManagers.filter(
      (m) => m.InviteeEmail != manager.InviteeEmail
    );

    this.cd.detectChanges();
  }
}
