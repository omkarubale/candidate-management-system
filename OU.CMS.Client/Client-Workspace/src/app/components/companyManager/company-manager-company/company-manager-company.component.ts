import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { CompanyService } from 'src/app/shared/api/company.service';
import { CompanyManagerDto, DeleteCompanyManagementDto, GetCompanyDto, GetCompanyManagementDto, RevokeCompanyManagementInviteDto, SaveCompanyDto } from 'src/app/shared/models/CompanyModels';
import { faClock, faEnvelope, faPlusSquare, faTrashAlt, faUser, faUserShield } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-company-manager-company',
  templateUrl: './company-manager-company.component.html',
  styleUrls: ['./company-manager-company.component.css'],
})
export class CompanyManagerCompanyComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private companyService: CompanyService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef
  ) {}

  companyDetails: GetCompanyDto;
  companyManagers: GetCompanyManagementDto;

  currentUserEmail: string;

  //icons
  adminIcon = faUserShield;
  userIcon = faUser;
  deleteIcon = faTrashAlt;
  addIcon = faPlusSquare;
  envelopeIcon = faEnvelope;

  ngOnInit(): void {
    this.currentUserEmail = this.accountService.userInfo.Email;
    this.companyService.getCompany(this.accountService.userInfo.CompanyId).subscribe(data => {
      this.companyDetails = data;
    });

    this.companyService.getCompanyManagement(this.accountService.userInfo.CompanyId).subscribe(data => {
      this.companyManagers = data;
    });

    console.log("on init", this.companyManagers.CompanyManagers);
  }

  editCompany(form: SaveCompanyDto) {
    this.companyService.saveCompany(form).subscribe(
      (result) => {
        this.toastr.success("The company was saved successfully.", 'Company Updated!');
      },
      (error) => {
        console.error(error);
        this.toastr.error(error.message, 'There was an error saving the company!');
      }
    );
  }

  removeManager(manager: CompanyManagerDto) {
    if(manager.HasAcceptedInvite) {
      let companyManager: DeleteCompanyManagementDto = {
        CompanyId: this.accountService.userInfo.CompanyId,
        UserId: manager.User.UserId
      };

      this.companyService.deleteCompanyManagement(companyManager).subscribe(
        (result) => {
          this.toastr.success("The manager was removed successfully.", 'Manager Deleted!');
        },
        (error) => {
          console.error(error);
          this.toastr.error(error.message, 'There was an error removing the company manager!');
        }
      );
    }
    else {
      let companyManagerInvite: RevokeCompanyManagementInviteDto = {
        CompanyId: this.accountService.userInfo.CompanyId,
        Email: manager.InviteeEmail
      };

      this.companyService.revokeCompanyManagementInvite(companyManagerInvite).subscribe(
        (result) => {
          this.toastr.success("The manager invite was removed successfully.", 'Manager Invite Deleted!');
        },
        (error) => {
          console.error(error);
          this.toastr.error(error.message, 'There was an error removing the company manager invite!');
        }
      );
    }

    this.companyManagers.CompanyManagers = this.companyManagers.CompanyManagers.filter(m => m.InviteeEmail != manager.InviteeEmail);

    this.cd.detectChanges();
  }
}
