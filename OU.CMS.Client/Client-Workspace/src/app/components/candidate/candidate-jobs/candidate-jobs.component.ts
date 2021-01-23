import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { CandidateService } from 'src/app/shared/api/candidate.service';
import { JobOpeningService } from 'src/app/shared/api/job-opening.service';
import { UserInfo } from 'src/app/shared/models/AuthenticationModels';
import { CreateCandidateDto } from 'src/app/shared/models/CandidateModels';
import { GetCandidateJobOpeningDto } from 'src/app/shared/models/JobOpeningModels';

@Component({
  selector: 'app-candidate-jobs',
  templateUrl: './candidate-jobs.component.html',
  styleUrls: ['./candidate-jobs.component.css'],
})
export class CandidateJobsComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private jobOpeningService: JobOpeningService,
    private candidateService: CandidateService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  jobs: GetCandidateJobOpeningDto[];

  ngOnInit(): void {
    this.fetchJobs();
  }

  fetchJobs() {
    this.jobOpeningService.getAllJobOpeningsForCandidate().subscribe((data) => {
      this.jobs = data;
    });
  }

  applyForJob(jobOpeningId: string, companyId: string) {
    var dto: CreateCandidateDto = {
      UserId: this.accountService.userInfo.UserId,
      JobOpeningId: jobOpeningId,
      CompanyId: companyId,
    };

    this.candidateService.createCandidate(dto).subscribe(
      (result) => {
        this.toastr.success(
          'You have applied for this Job successfully.',
          'Job Application Sent!'
        );

        this.fetchJobs();
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.ExceptionMessage,
          'There was an error applying for this job!'
        );
      }
    );
  }
}
