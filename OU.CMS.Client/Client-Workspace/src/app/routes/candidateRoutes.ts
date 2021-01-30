import { CandidateSigninComponent } from '../components/candidate/candidate-signin/candidate-signin.component';
import { CandidateSignupComponent } from '../components/candidate/candidate-signup/candidate-signup.component';
import { CandidateDashboardComponent } from '../components/candidate/candidate-dashboard/candidate-dashboard.component';
import { CandidateMyProfileComponent } from '../components/candidate/candidate-my-profile/candidate-my-profile.component';
import { CandidateJobsComponent } from '../components/candidate/candidate-jobs/candidate-jobs.component';
import { CandidateJobsJobComponent } from '../components/candidate/candidate-jobs-job/candidate-jobs-job.component';
import { AuthGuard } from '../shared/guards/auth.guard';

export const candidateRouterConfig = [
  {
    path: 'candidate-signup',
    component: CandidateSignupComponent
  },
  {
    path: 'candidate-signin',
    component: CandidateSigninComponent
  },
  {
    path: 'candidate-dashboard',
    component: CandidateDashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'candidate-myProfile',
    component: CandidateMyProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'candidate-jobs',
    component: CandidateJobsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'candidate-jobs-job/:jobOpeningId',
    component: CandidateJobsJobComponent,
    canActivate: [AuthGuard]
  }
]
