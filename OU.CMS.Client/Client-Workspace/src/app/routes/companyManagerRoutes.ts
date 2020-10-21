import { CompanyManagerDashboardComponent } from '../companyManager/company-manager-dashboard/company-manager-dashboard.component';
import { CompanyManagerLoginComponent } from '../companyManager/company-manager-login/company-manager-login.component';
import { CompanyManagerCompanyComponent } from '../companyManager/company-manager-company/company-manager-company.component';
import { CompanyManagerPositionsComponent } from '../companyManager/company-manager-positions/company-manager-positions.component';
import { CompanyManagerAssessmentsComponent } from '../companyManager/company-manager-assessments/company-manager-assessments.component';
import { CompanyManagerDrivesComponent } from '../companyManager/company-manager-drives/company-manager-drives.component';


export const companyManagerRouterConfig = [
  {
    path: 'companyManager-login',
    component: CompanyManagerLoginComponent
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
