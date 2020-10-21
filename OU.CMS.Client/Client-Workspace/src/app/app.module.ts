import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { candidateRouterConfig } from './routes/candidateRoutes';
import { companyManagerRouterConfig } from './routes/companyManagerRoutes';

import { CompaniesListComponent } from './company/companies-list/companies-list.component';
import { CompanyService } from './shared/api/company.service';
import { AccountService } from './shared/api/account.service';
import { CommonService } from './shared/api/common.service';
import { CompanyEditComponent } from './company/company-edit/company-edit.component';
import { HeaderComponent } from './common/header/header.component';
import { FooterComponent } from './common/footer/footer.component';
import { CandidateDashboardComponent } from './candidate/candidate-dashboard/candidate-dashboard.component';
import { CandidateMyProfileComponent } from './candidate/candidate-my-profile/candidate-my-profile.component';
import { CandidateJobsComponent } from './candidate/candidate-jobs/candidate-jobs.component';
import { CompanyManagerDashboardComponent } from './companyManager/company-manager-dashboard/company-manager-dashboard.component';
import { CompanyManagerCompanyComponent } from './companyManager/company-manager-company/company-manager-company.component';
import { CompanyManagerPositionsComponent } from './companyManager/company-manager-positions/company-manager-positions.component';
import { CompanyManagerAssessmentsComponent } from './companyManager/company-manager-assessments/company-manager-assessments.component';
import { CompanyManagerDrivesComponent } from './companyManager/company-manager-drives/company-manager-drives.component';
import { LoginComponent } from './common/login/login.component';
import { CandidateLoginComponent } from './candidate/candidate-login/candidate-login.component';
import { CompanyManagerLoginComponent } from './companyManager/company-manager-login/company-manager-login.component';
import { HomeComponent } from './common/home/home.component';

const appRoutes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home', pathMatch: 'full' },
  // Sign In/Sign Up Page
  {
    path: 'home',
    component: HomeComponent
  },

  // Candidate Pages
  ...candidateRouterConfig,

  // Company Manager Pages
  ...companyManagerRouterConfig,

  // To be deleted
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
  declarations: [
    AppComponent,
    CompaniesListComponent,
    CompanyEditComponent,
    HeaderComponent,
    FooterComponent,
    CandidateDashboardComponent,
    CandidateMyProfileComponent,
    CandidateJobsComponent,
    CompanyManagerDashboardComponent,
    CompanyManagerCompanyComponent,
    CompanyManagerPositionsComponent,
    CompanyManagerAssessmentsComponent,
    CompanyManagerDrivesComponent,
    LoginComponent,
    CandidateLoginComponent,
    CompanyManagerLoginComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [
    CompanyService,
    AccountService,
    CommonService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
