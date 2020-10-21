import { CandidateLoginComponent } from '../candidate/candidate-login/candidate-login.component';
import { CandidateDashboardComponent } from '../candidate/candidate-dashboard/candidate-dashboard.component';
import { CandidateMyProfileComponent } from '../candidate/candidate-my-profile/candidate-my-profile.component';
import { CandidateJobsComponent } from '../candidate/candidate-jobs/candidate-jobs.component';

export const candidateRouterConfig = [
  {
    path: 'candidate-login',
    component: CandidateLoginComponent
  },
  {
    path: 'candidate-dashboard',
    component: CandidateDashboardComponent
  },
  {
    path: 'candidate-myProfile',
    component: CandidateMyProfileComponent
  },
  {
    path: 'candidate-jobs',
    component: CandidateJobsComponent
  }
]
