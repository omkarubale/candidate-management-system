import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/common/home/home.component';
import { CompaniesListComponent } from './components/company/companies-list/companies-list.component';
import { CompanyEditComponent } from './components/company/company-edit/company-edit.component';
import { candidateRouterConfig } from './routes/candidateRoutes';
import { companyManagerRouterConfig } from './routes/companyManagerRoutes';

const appRoutes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  // Sign In/Sign Up Page
  {
    path: 'home',
    component: HomeComponent
  },

  // // Candidate Pages
  ...candidateRouterConfig,

  // Company Manager Pages
  ...companyManagerRouterConfig,

  // // To be deleted
  {
    path: 'company-list',
    component: CompaniesListComponent
  },
  {
    path: 'company-add',
    component: CompanyEditComponent
  },
  {
    path: 'company-edit/:id',
    component: CompanyEditComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
