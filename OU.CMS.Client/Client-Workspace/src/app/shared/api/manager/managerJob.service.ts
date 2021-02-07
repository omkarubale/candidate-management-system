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
import {
  CandidateTestDto,
  CandidateTestsContainerDto,
  CreateCandidateTestDto,
  GetCandidateDto,
  UpdateCandidateTestScoreDto,
} from '../../models/CandidateModels';

@Injectable()
export class ManagerJobService {
  public API = 'https://localhost:44305/api';
  public JOB_API = `${this.API}/managerJob`;

  constructor(private http: HttpClient) {}

  //#region JobOpening
  getJobOpenings(): Observable<GetJobOpeningCompanyDto[]> {
    return this.http.get<GetJobOpeningCompanyDto[]>(
      `${this.JOB_API}/GetJobOpenings`
    );
  }

  getJobOpening(jobOpeningId: string): Observable<GetJobOpeningDto> {
    return this.http.get<GetJobOpeningDto>(
      `${this.JOB_API}/GetJobOpening?jobOpeningId=${jobOpeningId}`
    );
  }

  createJobOpening(dto: CreateJobOpeningDto): Observable<GetJobOpeningDto> {
    return this.http.post<GetJobOpeningDto>(
      `${this.JOB_API}/CreateJobOpening`,
      dto
    );
  }

  updateJobOpening(dto: UpdateJobOpeningDto): Observable<GetJobOpeningDto> {
    return this.http.post<GetJobOpeningDto>(
      `${this.JOB_API}/UpdateJobOpening`,
      dto
    );
  }

  deleteJobOpening(jobOpeningId: string) {
    return this.http.delete(
      `${this.JOB_API}/DeleteJobOpening?jobOpeningId=${jobOpeningId}`
    );
  }
  //#endregion

  //#region Candidate
  getCandidate(candidateId: string): Observable<GetCandidateDto> {
    return this.http.get<GetCandidateDto>(
      `${this.JOB_API}/GetCandidate?candidateId=${candidateId}`
    );
  }

  getCandidatesForCompany(): Observable<GetCandidateDto[]> {
    return this.http.get<GetCandidateDto[]>(
      `${this.JOB_API}/GetCandidatesForCompany`
    );
  }

  getCandidatesForJobOpening(
    jobOpeningId: string
  ): Observable<GetCandidateDto[]> {
    return this.http.get<GetCandidateDto[]>(
      `${this.JOB_API}/GetCandidatesForJobOpening?jobOpeningId=${jobOpeningId}`
    );
  }

  getCandidateTestsAsCompanyManager(
    candidateId: string
  ): Observable<CandidateTestsContainerDto> {
    return this.http.get<CandidateTestsContainerDto>(
      `${this.JOB_API}/GetCandidateTestsAsCompanyManager?candidateId=${candidateId}`
    );
  }

  getCandidateTestAsCompanyManager(
    candidateId: string,
    testId: string
  ): Observable<CandidateTestDto> {
    return this.http.get<CandidateTestDto>(
      `${this.JOB_API}/GetCandidateTestAsCompanyManager?candidateId=${candidateId}&testId=${testId}`
    );
  }

  deleteCandidate(candidateId: string) {
    return this.http.delete(
      `${this.JOB_API}/DeleteCandidate?candidate=${candidateId}`
    );
  }
  //#endregion

  //#region Candidate Tests
  getTestCandidatesAsCompanyManager(
    testId: string
  ): Observable<CandidateTestDto[]> {
    return this.http.get<CandidateTestDto[]>(
      `${this.JOB_API}/GetTestCandidatesAsCompanyManager?testId=${testId}`
    );
  }

  createCandidateTest(
    dto: CreateCandidateTestDto
  ): Observable<CandidateTestDto> {
    return this.http.post<CandidateTestDto>(
      `${this.JOB_API}/CreateCandidateTest`,
      dto
    );
  }

  updateCandidateTestScore(dto: UpdateCandidateTestScoreDto) {
    return this.http.post(`${this.JOB_API}/UpdateCandidateTestScore`, dto);
  }
  //#endregion
}
