import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import {
  CreateJobOpeningDto,
  GetCandidateJobOpeningDto,
  GetJobOpeningCompanyDto,
  GetJobOpeningDto,
  UpdateJobOpeningDto,
} from '../../models/JobOpeningModels';

@Injectable()
export class CandidateJobService {
  public API = 'https://localhost:44305/api';
  public JOB_OPENING_API = `${this.API}/candidateJob`;

  constructor(private http: HttpClient) {}

  //#region Job Opening for Candidate
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
  //#endregion
}
