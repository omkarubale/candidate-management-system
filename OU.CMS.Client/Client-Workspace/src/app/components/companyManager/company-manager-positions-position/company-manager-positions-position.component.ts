import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { CandidateService } from 'src/app/shared/api/candidate.service';
import { JobOpeningService } from 'src/app/shared/api/job-opening.service';
import { GetCandidateDto } from 'src/app/shared/models/CandidateModels';
import { GetJobOpeningDto, UpdateJobOpeningDto } from 'src/app/shared/models/JobOpeningModels';

@Component({
  selector: 'app-company-manager-positions-position',
  templateUrl: './company-manager-positions-position.component.html',
  styleUrls: ['./company-manager-positions-position.component.css'],
})
export class CompanyManagerPositionsPositionComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private jobOpeningService: JobOpeningService,
    private candidateService: CandidateService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  currentPositionId: string;
  positionDetails: GetJobOpeningDto;
  positionCandidates: GetCandidateDto[];

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.currentPositionId = params['id'];

      if (this.currentPositionId) {
        this.jobOpeningService
          .getJobOpening(this.currentPositionId)
          .subscribe((data) => {
            if (data) {
              this.positionDetails = data;
            } else {
              console.log(
                `Position with id '${this.currentPositionId} was not found!'`
              );
              this.gotoPositionsIndex();
            }
          });

        this.fetchPositionCandidates();
      } else {
        this.gotoPositionsIndex();
      }
    });
  }

  // Position
  editPosition(form: UpdateJobOpeningDto) {
    this.jobOpeningService.updateJobOpening(form).subscribe(
      (result) => {
        this.toastr.success(
          'The job position was saved successfully.',
          'Job Opening Updated!'
        );
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.message,
          'There was an error saving the job Opening!'
        );
      }
    );
  }

  gotoPositionsIndex() {
    this.router.navigate(['/companyManager-positions']);
  }

  // Position Candidates
  fetchPositionCandidates() {
    this.candidateService
    .getCandidatesForJobOpening(this.currentPositionId)
    .subscribe((data) => {
      this.positionCandidates = data;
    });
  }
}
