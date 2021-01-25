import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faPlusSquare } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { ManagerTestService } from 'src/app/shared/api/manager/managerTest.service';
import { CreateTestDto, GetTestDto } from 'src/app/shared/models/TestModels';

@Component({
  selector: 'app-company-manager-assessments',
  templateUrl: './company-manager-assessments.component.html',
  styleUrls: ['./company-manager-assessments.component.css']
})
export class CompanyManagerAssessmentsComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private managerTestService: ManagerTestService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private modalService: NgbModal,
    private router: Router
  ) {}

  //icons
  addIcon = faPlusSquare;

  assessments: GetTestDto[];
  addAssessmentForm: CreateTestDto;
  addAssessmentModal: NgbModalRef;

  ngOnInit(): void {
    this.addAssessmentForm = new CreateTestDto();

    this.fetchTests();
  }

  fetchTests() {
    this.managerTestService
    .getTestsAsCompanyManager()
    .subscribe((data) => {
      this.assessments = data;
    });
  }

  openAddAssessmentModal(content) {
    this.addAssessmentModal = this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  addAssessment(form: CreateTestDto) {
    this.managerTestService.createTest(form).subscribe(
      (result) => {
        this.toastr.success(
          'The Assessment was created successfully.',
          'Assessment Created!'
        );

        this.fetchTests();

        this.cd.detectChanges();
        this.addAssessmentModal.close();
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.ExceptionMessage,
          'There was an error creating this assessment!'
        );
      }
    );
  }

}
