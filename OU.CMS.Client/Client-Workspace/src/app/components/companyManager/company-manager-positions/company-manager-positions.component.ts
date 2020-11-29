import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faPlusSquare } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { JobOpeningService } from 'src/app/shared/api/job-opening.service';
import { CreateJobOpeningDto, GetJobOpeningCompanyDto } from 'src/app/shared/models/JobOpeningModels';

@Component({
  selector: 'app-company-manager-positions',
  templateUrl: './company-manager-positions.component.html',
  styleUrls: ['./company-manager-positions.component.css']
})
export class CompanyManagerPositionsComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private jobOpeningService: JobOpeningService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private modalService: NgbModal,
    private router: Router
  ) {}

  //icons
  addIcon = faPlusSquare;

  positions: GetJobOpeningCompanyDto[];
  addPositionForm: CreateJobOpeningDto;
  addPositionModal: NgbModalRef;

  ngOnInit(): void {
    this.addPositionForm = new CreateJobOpeningDto();
    this.addPositionForm.CompanyId = this.accountService.userInfo.CompanyId;

    this.fetchPositions();
  }

  fetchPositions() {
    this.jobOpeningService
    .getAllJobOpeningsForCompany(this.accountService.userInfo.CompanyId)
    .subscribe((data) => {
      this.positions = data;
    });
  }

  openAddPositionModal(content) {
    this.addPositionForm.CompanyId = this.accountService.userInfo.CompanyId;

    this.addPositionModal = this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  addJobOpening(form: CreateJobOpeningDto) {
    this.jobOpeningService.createJobOpening(form).subscribe(
      (result) => {
        this.toastr.success(
          'The Job Opening was created successfully.',
          'Job Opening Created!'
        );

        this.fetchPositions();

        this.cd.detectChanges();
        this.addPositionModal.close();
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.ExceptionMessage,
          'There was an error creating this job opening!'
        );
      }
    );
  }
}
