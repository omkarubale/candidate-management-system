import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faPlusSquare } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { TestService } from 'src/app/shared/api/test.service';
import { CreateTestDto, GetTestDto } from 'src/app/shared/models/TestModels';

@Component({
  selector: 'app-company-manager-assessments',
  templateUrl: './company-manager-assessments.component.html',
  styleUrls: ['./company-manager-assessments.component.css']
})
export class CompanyManagerAssessmentsComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private testService: TestService,
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
    this.testService
    .getAllTestsAsCompanyManager()
    .subscribe((data) => {
      this.assessments = data;
    });
  }

  openAddAssessmentModal(content) {
    this.addAssessmentModal = this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  addAssessment(form: CreateTestDto) {
    this.testService.createTest(form).subscribe(
      (result) => {
        this.toastr.success(
          'The Test was created successfully.',
          'Test Created!'
        );

        this.fetchTests();

        this.cd.detectChanges();
        this.addAssessmentModal.close();
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.ExceptionMessage,
          'There was an error creating this test!'
        );
      }
    );
  }

}
