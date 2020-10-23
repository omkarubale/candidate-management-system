import { CompanyManagerDashboardComponent } from '../components/companyManager/company-manager-dashboard/company-manager-dashboard.component';
import { CompanyManagerSignupComponent } from '../components/companyManager/company-manager-signup/company-manager-signup.component';
import { CompanyManagerSigninComponent } from '../components/companyManager/company-manager-signin/company-manager-signin.component';
import { CompanyManagerCompanyComponent } from '../components/companyManager/company-manager-company/company-manager-company.component';
import { CompanyManagerPositionsComponent } from '../components/companyManager/company-manager-positions/company-manager-positions.component';
import { CompanyManagerAssessmentsComponent } from '../components/companyManager/company-manager-assessments/company-manager-assessments.component';
import { CompanyManagerDrivesComponent } from '../components/companyManager/company-manager-drives/company-manager-drives.component';


export const companyManagerRouterConfig = [
  {
    path: 'companyManager-signup',
    component: CompanyManagerSignupComponent
  },
  {
    path: 'companyManager-signin',
    component: CompanyManagerSigninComponent
  },
  {
    path: 'companyManager-dashboard',
    component: CompanyManagerDashboardComponent
  },
  {
    path: 'companyManager-company',
    component: CompanyManagerCompanyComponent
  },
  {
    path: 'companyManager-positions',
    component: CompanyManagerPositionsComponent
  },
  {
    path: 'companyManager-assessments',
    component: CompanyManagerAssessmentsComponent
  },
  {
    path: 'companyManager-drives',
    component: CompanyManagerDrivesComponent
  },

]
