import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobOpeningService } from 'src/app/shared/api/job-opening.service';
import { GetCandidateJobOpeningDto } from 'src/app/shared/models/JobOpeningModels';

@Component({
  selector: 'app-candidate-jobs-job',
  templateUrl: './candidate-jobs-job.component.html',
  styleUrls: ['./candidate-jobs-job.component.css']
})
export class CandidateJobsJobComponent implements OnInit {

  constructor(
    private jobOpeningService: JobOpeningService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  currentJobId: string;
  job: GetCandidateJobOpeningDto;

  ngOnInit(): void {
    this.fetchJob();
  }

  fetchJob() {
    this.route.params.subscribe((params) => {
      this.currentJobId = params['jobOpeningId'];

      if (this.currentJobId) {
        this.jobOpeningService
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
