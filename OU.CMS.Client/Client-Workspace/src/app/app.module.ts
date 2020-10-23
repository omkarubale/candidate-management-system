import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { CompaniesListComponent } from './components/company/companies-list/companies-list.component';
import { CompanyService } from './shared/api/company.service';
import { AccountService } from './shared/api/account.service';
import { CommonService } from './shared/api/common.service';
import { CompanyEditComponent } from './components/company/company-edit/company-edit.component';
import { NavComponent } from './components/common/nav/nav.component';
import { FooterComponent } from './components/common/footer/footer.component';
import { CandidateDashboardComponent } from './components/candidate/candidate-dashboard/candidate-dashboard.component';
import { CandidateMyProfileComponent } from './components/candidate/candidate-my-profile/candidate-my-profile.component';
import { CandidateJobsComponent } from './components/candidate/candidate-jobs/candidate-jobs.component';
import { CompanyManagerDashboardComponent } from './components/companyManager/company-manager-dashboard/company-manager-dashboard.component';
import { CompanyManagerCompanyComponent } from './components/companyManager/company-manager-company/company-manager-company.component';
import { CompanyManagerPositionsComponent } from './components/companyManager/company-manager-positions/company-manager-positions.component';
import { CompanyManagerAssessmentsComponent } from './components/companyManager/company-manager-assessments/company-manager-assessments.component';
import { CompanyManagerDrivesComponent } from './components/companyManager/company-manager-drives/company-manager-drives.component';
import { HomeComponent } from './components/common/home/home.component';
import { CandidateSignupComponent } from './components/candidate/candidate-signup/candidate-signup.component';
import { CandidateSigninComponent } from './components/candidate/candidate-signin/candidate-signin.component';
import { CompanyManagerSigninComponent } from './components/companyManager/company-manager-signin/company-manager-signin.component';
import { CompanyManagerSignupComponent } from './components/companyManager/company-manager-signup/company-manager-signup.component';
import { SigninFormComponent } from './components/account/signin-form/signin-form.component';
import { SignupFormComponent } from './components/account/signup-form/signup-form.component';

@NgModule({
  declarations: [
    AppComponent,
    CompaniesListComponent,
    CompanyEditComponent,
    NavComponent,
    FooterComponent,
    CandidateDashboardComponent,
    CandidateMyProfileComponent,
    CandidateJobsComponent,
    CompanyManagerDashboardComponent,
    CompanyManagerCompanyComponent,
    CompanyManagerPositionsComponent,
    CompanyManagerAssessmentsComponent,
    CompanyManagerDrivesComponent,
    HomeComponent,
    CandidateSignupComponent,
    CandidateSigninComponent,
    CompanyManagerSigninComponent,
    CompanyManagerSignupComponent,
    SigninFormComponent,
    SignupFormComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
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
