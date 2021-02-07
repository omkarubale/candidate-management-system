import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faPlusSquare, faTrash } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { ManagerJobService } from 'src/app/shared/api/manager/managerJob.service';
import { ManagerTestService } from 'src/app/shared/api/manager/managerTest.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { CandidateTestDto } from 'src/app/shared/models/CandidateModels';
import { CreateTestScoreDto, GetTestDto, UpdateTestDto } from 'src/app/shared/models/TestModels';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-company-manager-assessments-assessment',
  templateUrl: './company-manager-assessments-assessment.component.html',
  styleUrls: ['./company-manager-assessments-assessment.component.css']
})
export class CompanyManagerAssessmentsAssessmentComponent implements OnInit {

  constructor(
    private managerTestService: ManagerTestService,
    private managerJobService: ManagerJobService,
    private navbarService: NavbarService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private modalService: NgbModal,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  //icons
  addIcon = faPlusSquare;
  deleteIcon = faTrash;

  currentAssessmentId: string;
  assessmentDetails: GetTestDto = new GetTestDto;
  assessmentCandidates: CandidateTestDto[];

  addAssessmentScoreModal: NgbModalRef;
  addAssessmentScoreForm: CreateTestScoreDto;

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Assessments);

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
      this.managerTestService
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
      this.managerTestService.updateTest(form).subscribe(
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

    deleteAssessment() {
      this.managerTestService.deleteTest(this.currentAssessmentId).subscribe(
        () => {
          this.toastr.success(
            'The assessment was deleted successfully.',
            'Assessment Deleted!'
          );
          this.gotoAssessmentsIndex();
        },
        (error) => {
          console.error(error);
          this.toastr.error(
            error.message,
            'There was an error deleting the Assessment!'
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
      this.managerTestService.createTestScore(form).subscribe(
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
      this.managerJobService
      .getTestCandidatesAsCompanyManager(this.currentAssessmentId)
      .subscribe((data) => {
        this.assessmentCandidates = data;
      });
    }

}
