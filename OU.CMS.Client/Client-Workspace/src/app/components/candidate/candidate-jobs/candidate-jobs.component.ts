import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { CandidateJobService } from 'src/app/shared/api/candidate/candidateJob.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { UserInfo } from 'src/app/shared/models/AuthenticationModels';
import { CreateCandidateDto } from 'src/app/shared/models/CandidateModels';
import { GetCandidateJobOpeningDto } from 'src/app/shared/models/JobOpeningModels';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-candidate-jobs',
  templateUrl: './candidate-jobs.component.html',
  styleUrls: ['./candidate-jobs.component.css'],
})
export class CandidateJobsComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private candidateJobService: CandidateJobService,
    private navbarService: NavbarService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  jobs: GetCandidateJobOpeningDto[];

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Jobs);

    this.fetchJobs();
  }

  fetchJobs() {
    this.candidateJobService.getAllJobOpeningsForCandidate().subscribe((data) => {
      this.jobs = data;
    });
  }

  applyForJob(jobOpeningId: string, companyId: string) {
    var dto: CreateCandidateDto = {
      UserId: this.accountService.userInfo.UserId,
      JobOpeningId: jobOpeningId,
      CompanyId: companyId,
    };

    this.candidateJobService.createCandidate(dto).subscribe(
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
