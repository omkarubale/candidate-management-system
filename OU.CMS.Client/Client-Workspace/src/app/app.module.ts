import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { CompaniesListComponent } from './company/companies-list/companies-list.component';
import { CompanyService } from './shared/api/company.service';
import { CompanyEditComponent } from './company/company-edit/company-edit.component';
import { HeaderComponent } from './common/header/header.component';
import { FooterComponent } from './common/footer/footer.component';

const appRoutes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  // Sign In/Sign Up Page
  // {
  //   path: 'login',
  //   component: LoginComponent
  // },

  // Candidate Pages
  // {
  //   path: 'candidate-dashboard',
  //   component: CandidateDashboardComponent
  // },
  // {
  //   path: 'candidate-myProfile',
  //   component: CandidateMyProfileComponent
  // },
  // {
  //   path: 'candidate-jobs',
  //   component: CandidateJobsComponent
  // },

  // Company Manager Pages
  // {
  //   path: 'companyManager-dashboard',
  //   component: CompanyManagerDashboardComponent
  // },
  // {
  //   path: 'companyManager-company',
  //   component: CompanyManagerCompanyComponent
  // },
  // {
  //   path: 'companyManager-positions',
  //   component: CompanyManagerPositionsComponent
  // },
  // {
  //   path: 'companyManager-assessments',
  //   component: CompanyManagerAssessmentsComponent
  // },
  // {
  //   path: 'companyManager-drives',
  //   component: CompanyManagerDrivesComponent
  // },

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
    FooterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [CompanyService],
  bootstrap: [AppComponent]
})
export class AppModule { }
