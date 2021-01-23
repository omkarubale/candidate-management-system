import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

//#region Third party modules
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
//#endregion

//#region Custom Services
import { CompanyService } from './shared/api/company.service';
import { AccountService } from './shared/api/account.service';
import { CommonService } from './shared/api/common.service';
import { CandidateService } from './shared/api/candidate.service';
import { JobOpeningService } from './shared/api/job-opening.service';
import { TestService } from './shared/api/test.service';
import { UserService } from './shared/api/user.service';
//#endregion

//#region Providers
import { AuthInterceptorProvider } from './shared/interceptors/auth.interceptor';
import { ErrorInterceptorProvider } from './shared/interceptors/error.interceptor';
//#endregion

//#region Components
// Common
import { NavComponent } from './components/common/nav/nav.component';
import { FooterComponent } from './components/common/footer/footer.component';
import { HomeComponent } from './components/common/home/home.component';
import { SigninFormComponent } from './components/account/signin-form/signin-form.component';
import { SignupFormComponent } from './components/account/signup-form/signup-form.component';

// Candidate
import { CandidateDashboardComponent } from './components/candidate/candidate-dashboard/candidate-dashboard.component';
import { CandidateMyProfileComponent } from './components/candidate/candidate-my-profile/candidate-my-profile.component';
import { CandidateJobsComponent } from './components/candidate/candidate-jobs/candidate-jobs.component';
import { CandidateSignupComponent } from './components/candidate/candidate-signup/candidate-signup.component';
import { CandidateSigninComponent } from './components/candidate/candidate-signin/candidate-signin.component';

// CompanyManager
import { CompanyManagerDashboardComponent } from './components/companyManager/company-manager-dashboard/company-manager-dashboard.component';
import { CompanyManagerCompanyComponent } from './components/companyManager/company-manager-company/company-manager-company.component';
import { CompanyManagerPositionsComponent } from './components/companyManager/company-manager-positions/company-manager-positions.component';
import { CompanyManagerAssessmentsComponent } from './components/companyManager/company-manager-assessments/company-manager-assessments.component';
import { CompanyManagerDrivesComponent } from './components/companyManager/company-manager-drives/company-manager-drives.component';
import { CompanyManagerSigninComponent } from './components/companyManager/company-manager-signin/company-manager-signin.component';
import { CompanyManagerSignupComponent } from './components/companyManager/company-manager-signup/company-manager-signup.component';
import { CompanyManagerPositionsPositionComponent } from './components/companyManager/company-manager-positions-position/company-manager-positions-position.component';
import { CompanyManagerPositionsCandidateComponent } from './components/companyManager/company-manager-positions-candidate/company-manager-positions-candidate.component';
import { CompanyManagerAssessmentsAssessmentComponent } from './components/companyManager/company-manager-assessments-assessment/company-manager-assessments-assessment.component';
import { CompanyManagerAssessmentCandidateListComponent } from './components/companyManager/company-manager-assessment-candidate-list/company-manager-assessment-candidate-list.component';
import { CompanyManagerPositionCandidateListComponent } from './components/companyManager/company-manager-position-candidate-list/company-manager-position-candidate-list.component';
import { CandidateJobsJobComponent } from './components/candidate/candidate-jobs-job/candidate-jobs-job.component';
//#endregion

@NgModule({
  declarations: [
    AppComponent,

    // Common
    NavComponent,
    FooterComponent,
    HomeComponent,
    SigninFormComponent,
    SignupFormComponent,

    // Candidate
    CandidateSignupComponent,
    CandidateSigninComponent,
    CandidateDashboardComponent,
    CandidateMyProfileComponent,
    CandidateJobsComponent,

    // CompanyManager
    CompanyManagerSigninComponent,
    CompanyManagerSignupComponent,
    CompanyManagerDashboardComponent,
    CompanyManagerCompanyComponent,
    CompanyManagerPositionsComponent,
    CompanyManagerAssessmentsComponent,
    CompanyManagerDrivesComponent,
    CompanyManagerPositionsPositionComponent,
    CompanyManagerPositionsCandidateComponent,
    CompanyManagerAssessmentsAssessmentComponent,
    CompanyManagerAssessmentCandidateListComponent,
    CompanyManagerPositionCandidateListComponent,
    CandidateJobsJobComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    FontAwesomeModule,
    NgbModule,
  ],
  providers: [
    // Custom Services
    CompanyService,
    AccountService,
    CommonService,
    CandidateService,
    JobOpeningService,
    TestService,
    UserService,

    //Providers
    AuthInterceptorProvider,
    ErrorInterceptorProvider,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
