import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CandidateJobService } from 'src/app/shared/api/candidate/candidateJob.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { GetCandidateJobOpeningDto } from 'src/app/shared/models/JobOpeningModels';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-candidate-jobs-job',
  templateUrl: './candidate-jobs-job.component.html',
  styleUrls: ['./candidate-jobs-job.component.css']
})
export class CandidateJobsJobComponent implements OnInit {

  constructor(
    private candidateJobService: CandidateJobService,
    private navbarService: NavbarService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  currentJobId: string;
  job: GetCandidateJobOpeningDto;

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Jobs);

    this.fetchJob();
  }

  fetchJob() {
    this.route.params.subscribe((params) => {
      this.currentJobId = params['jobOpeningId'];

      if (this.currentJobId) {
        this.candidateJobService
          .getJobOpeningForCandidate(this.currentJobId)
          .subscribe((data) => {
            if (data) {
              this.job = data;
            } else {
              console.log(
                `Position with id '${this.currentJobId} was not found!'`
              );
              this.gotoJobsIndex();
            }
          });
      } else {
        this.gotoJobsIndex();
      }
    });
  }

  gotoJobsIndex() {
    this.router.navigate(['/candidate-jobs']);
  }
}
