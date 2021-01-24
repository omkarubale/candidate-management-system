import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faPlusSquare } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { CandidateService } from 'src/app/shared/api/candidate.service';
import { TestService } from 'src/app/shared/api/test.service';
import { CandidateTestDto } from 'src/app/shared/models/CandidateModels';
import { CreateTestScoreDto, GetTestDto, UpdateTestDto } from 'src/app/shared/models/TestModels';

@Component({
  selector: 'app-company-manager-assessments-assessment',
  templateUrl: './company-manager-assessments-assessment.component.html',
  styleUrls: ['./company-manager-assessments-assessment.component.css']
})
export class CompanyManagerAssessmentsAssessmentComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private testService: TestService,
    private candidateService: CandidateService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private modalService: NgbModal,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  //icons
  addIcon = faPlusSquare;

  currentAssessmentId: string;
  assessmentDetails: GetTestDto;
  assessmentCandidates: CandidateTestDto[];

  addAssessmentScoreModal: NgbModalRef;
  addAssessmentScoreForm: CreateTestScoreDto;

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.currentAssessmentId = params['id'];

      if (this.currentAssessmentId) {
        this.fetchAssessment();
        this.fetchAssessmentCandidates();
      } else {
        this.gotoAssessmentsIndex();
      }
    });

    this.addAssessmentScoreForm = new CreateTestScoreDto();
  }

    // Assessment
    fetchAssessment() {
      this.testService
        .getTestAsCompanyManager(this.currentAssessmentId)
        .subscribe((data) => {
          if (data) {
            this.assessmentDetails = data;
          } else {
            console.log(
              `Position with id '${this.currentAssessmentId} was not found!'`
            );
            this.gotoAssessmentsIndex();
          }
        });
    }

    editAssessment(form: UpdateTestDto) {
      this.testService.updateTest(form).subscribe(
        (result) => {
          this.toastr.success(
            'The assessment was saved successfully.',
            'Assessment Updated!'
          );
        },
        (error) => {
          console.error(error);
          this.toastr.error(
            error.message,
            'There was an error saving the Assessment!'
          );
        }
      );
    }

    gotoAssessmentsIndex() {
      this.router.navigate(['/companyManager-assessments']);
    }

    // Assessment Scores
    openAddAssessmentScoreModal(content) {
      this.addAssessmentScoreModal = this.modalService
        .open(content, { ariaLabelledBy: 'modal-basic-title' });
    }

    addAssessmentScore(form: CreateTestScoreDto) {
      this.testService.createTestScore(form).subscribe(
        (result) => {
          this.toastr.success(
            'The Assessment Score was created successfully.',
            'Assessment Score Created!'
          );

          this.fetchAssessment();

          this.cd.detectChanges();
          this.addAssessmentScoreModal.close();
        },
        (error) => {
          console.error(error);
          this.toastr.error(
            error.ExceptionMessage,
            'There was an error creating this assessment score!'
          );
        }
      );
    }

    // Assessment Candidates
    fetchAssessmentCandidates() {
      this.candidateService
      .getCandidateTestsAsCompanyManager(this.currentAssessmentId)
      .subscribe((data) => {
        this.assessmentCandidates = data;
      });
    }

}
