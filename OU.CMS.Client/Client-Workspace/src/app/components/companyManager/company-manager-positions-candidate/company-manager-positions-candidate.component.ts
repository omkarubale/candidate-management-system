import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { faPlusSquare } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { ManagerJobService } from 'src/app/shared/api/manager/managerJob.service';
import { ManagerTestService } from 'src/app/shared/api/manager/managerTest.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { CandidateTestDto, CandidateTestsContainerDto, CreateCandidateTestDto, GetCandidateDto } from 'src/app/shared/models/CandidateModels';
import { LookupDto } from 'src/app/shared/models/CommonModels';
import { JobOpeningSimpleDto } from 'src/app/shared/models/JobOpeningModels';
import { UserSimpleDto } from 'src/app/shared/models/UserModels';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-company-manager-positions-candidate',
  templateUrl: './company-manager-positions-candidate.component.html',
  styleUrls: ['./company-manager-positions-candidate.component.css']
})
export class CompanyManagerPositionsCandidateComponent implements OnInit {

  constructor(
    private navbarService: NavbarService,
    private managerJobService: ManagerJobService,
    private managerTestService: ManagerTestService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private cd: ChangeDetectorRef,
    private modalService: NgbModal,
  ) {
    this.candidateDetails = new CandidateTestsContainerDto;
    this.candidateDetails.Candidate = new GetCandidateDto;
    this.candidateDetails.Candidate.JobOpening = new JobOpeningSimpleDto;
    this.candidateDetails.Candidate.User = new UserSimpleDto;
  }

  //icons
  addIcon = faPlusSquare;

  currentPositionId: string;
  currentCandidateId: string;

  candidateDetails: CandidateTestsContainerDto;

  addCandidateAssessmentModal: NgbModalRef;
  addCandidateAssessmentFormObject: CreateCandidateTestDto;

  companyTestsList: LookupDto[];

  ngOnInit(): void {

    this.route.params.subscribe((params) => {
    this.currentPositionId = params['id'];
    this.currentCandidateId = params['candidateId'];

    this.navbarService.setCurrentTab(NavbarTabs.Positions);

      if (this.currentCandidateId) {
        this.fetchCandidate();
      } else {
        this.gotoSourceIndex();
      }
    });
  }

  // Candidate
  fetchCandidate() {
    this.managerJobService
      .getCandidateTestsAsCompanyManager(this.currentCandidateId)
      .subscribe((data) => {
        if (data) {
          this.candidateDetails = data;
        } else {
          console.log(
            `Candidate with id '${this.currentCandidateId} was not found!'`
          );
          this.gotoSourceIndex();
        }
      });
  }

  // Assessment Scores
  openAddCandidateAssessmentModal(content) {
    this.addCandidateAssessmentFormObject = new CreateCandidateTestDto();
    this.addCandidateAssessmentFormObject.CandidateId = this.currentCandidateId;

    this.managerTestService.getTestsAsCompanyManagerForLookup().subscribe(
      (result) => {
        this.companyTestsList = result;
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.ExceptionMessage,
          'There was an error loading the assessments in this company!'
        );
      }
    );

    this.addCandidateAssessmentModal = this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  addCandidateAssessment(form: CreateCandidateTestDto) {
    this.managerJobService.createCandidateTest(form).subscribe(
      (result) => {
        this.toastr.success(
          'The Candidate Assessment was created successfully.',
          'Candidate Assessment Created!'
        );

        this.fetchCandidate();

        this.cd.detectChanges();
        this.addCandidateAssessmentModal.close();
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.ExceptionMessage,
          'There was an error creating this candidate assessment!'
        );
      }
    );
  }

  // Go to Index
  gotoSourceIndex() {
      this.router.navigate(['/companyManager-positions']);
  }
}
