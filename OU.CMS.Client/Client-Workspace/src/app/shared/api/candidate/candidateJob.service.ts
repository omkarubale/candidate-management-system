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
import { CandidateTestDto, CreateCandidateDto, GetCandidateDto } from '../../models/CandidateModels';

@Injectable()
export class CandidateJobService {
  public API = 'https://localhost:44305/api';
  public JOB_API = `${this.API}/candidateJob`;

  constructor(private http: HttpClient) {}

  //#region Job Opening for Candidate
  getAllJobOpeningsForCandidate(): Observable<GetCandidateJobOpeningDto[]> {
    return this.http.get<GetCandidateJobOpeningDto[]>(
      `${this.JOB_API}/GetAllJobOpeningsForCandidate`
    );
  }

  getJobOpeningForCandidate(jobOpeningId: string): Observable<GetCandidateJobOpeningDto> {
    return this.http.get<GetCandidateJobOpeningDto>(
      `${this.JOB_API}/GetJobOpeningForCandidate?jobOpeningId=${jobOpeningId}`
    );
  }
  //#endregion

  //#region Candidate
  getCandidate(candidateId: string) : Observable<GetCandidateDto> {
    return this.http.get<GetCandidateDto>(
      `${this.JOB_API}/GetCandidate?candidateId=${candidateId}`
    );
  }

  createCandidate(dto: CreateCandidateDto) : Observable<GetCandidateDto> {
    return this.http.post<GetCandidateDto>(
      `${this.JOB_API}/CreateCandidate`, dto
    );
  }

  getCandidatesForUser() : Observable<GetCandidateDto[]> {
    return this.http.get<GetCandidateDto[]>(
      `${this.JOB_API}/GetCandidatesForUser`
    );
  }

  deleteCandidate(candidateId: string) {
    return this.http.delete(
      `${this.JOB_API}/DeleteCandidate?candidate=${candidateId}`
    );
  }

  getCandidateTestAsCandidate(candidateId: string, testId: string) : Observable<CandidateTestDto> {
    return this.http.get<CandidateTestDto>(
      `${this.JOB_API}/GetCandidate?candidateId=${candidateId}&testId=${testId}`
    );
  }
  //#endregion
}
