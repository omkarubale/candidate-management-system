import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import {
  CreateJobOpeningDto,
  GetCandidateJobOpeningDto,
  GetJobOpeningCompanyDto,
  GetJobOpeningDto,
  UpdateJobOpeningDto,
} from '../models/JobOpeningModels';

@Injectable()
export class JobOpeningService {
  public API = 'https://localhost:44305/api';
  public JOB_OPENING_API = `${this.API}/jobPosition`;

  constructor(private http: HttpClient) {}

  //#region JobOpening
  getAllJobOpeningsForCompany(
    companyId: string
  ): Observable<GetJobOpeningCompanyDto[]> {
    return this.http.get<GetJobOpeningCompanyDto[]>(
      `${this.JOB_OPENING_API}/GetAllJobOpeningsForCompany?companyId=${companyId}`
    );
  }

  getAllJobOpeningsForCandidate(): Observable<GetCandidateJobOpeningDto[]> {
    return this.http.get<GetCandidateJobOpeningDto[]>(
      `${this.JOB_OPENING_API}/GetAllJobOpeningsForCandidate`
    );
  }

  getJobOpeningForCandidate(jobOpeningId: string): Observable<GetCandidateJobOpeningDto> {
    return this.http.get<GetCandidateJobOpeningDto>(
      `${this.JOB_OPENING_API}/GetJobOpeningForCandidate?jobOpeningId=${jobOpeningId}`
    );
  }

  getJobOpening(jobOpeningId: string): Observable<GetJobOpeningDto> {
    return this.http.get<GetJobOpeningDto>(
      `${this.JOB_OPENING_API}/GetJobOpening?jobOpeningId=${jobOpeningId}`
    );
  }

  createJobOpening(
    jobOpening: CreateJobOpeningDto
  ): Observable<GetJobOpeningDto> {
    return this.http.post<GetJobOpeningDto>(
      `${this.JOB_OPENING_API}/CreateJobOpening`,
      jobOpening
    );
  }

  updateJobOpening(
    jobOpening: UpdateJobOpeningDto
  ): Observable<GetJobOpeningDto> {
    return this.http.post<GetJobOpeningDto>(
      `${this.JOB_OPENING_API}/UpdateJobOpening`,
      jobOpening
    );
  }
  //#endregion
}
