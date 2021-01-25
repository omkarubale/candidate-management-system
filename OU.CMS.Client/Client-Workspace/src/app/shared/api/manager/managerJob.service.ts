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
export class ManagerJobService {
  public API = 'https://localhost:44305/api';
  public JOB_OPENING_API = `${this.API}/managerJob`;

  constructor(private http: HttpClient) {}

  //#region JobOpening
  getJobOpenings(): Observable<GetJobOpeningCompanyDto[]> {
    return this.http.get<GetJobOpeningCompanyDto[]>(
      `${this.JOB_OPENING_API}/GetJobOpenings`
    );
  }

  getJobOpening(jobOpeningId: string): Observable<GetJobOpeningDto> {
    return this.http.get<GetJobOpeningDto>(
      `${this.JOB_OPENING_API}/GetJobOpening?jobOpeningId=${jobOpeningId}`
    );
  }

  createJobOpening(
    dto: CreateJobOpeningDto
  ): Observable<GetJobOpeningDto> {
    return this.http.post<GetJobOpeningDto>(
      `${this.JOB_OPENING_API}/CreateJobOpening`,
      dto
    );
  }

  updateJobOpening(
    dto: UpdateJobOpeningDto
  ): Observable<GetJobOpeningDto> {
    return this.http.post<GetJobOpeningDto>(
      `${this.JOB_OPENING_API}/UpdateJobOpening`,
      dto
    );
  }

  deleteJobOpening(jobOpeningId: string) {
    return this.http.delete(
      `${this.JOB_OPENING_API}/DeleteJobOpening?jobOpeningId=${jobOpeningId}`
    );
  }
  //#endregion
}
