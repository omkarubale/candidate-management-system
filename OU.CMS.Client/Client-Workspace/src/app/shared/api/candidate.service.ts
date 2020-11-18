import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import {
  CandidateTestDto,
  CreateCandidateDto,
  CreateCandidateTestDto,
  GetCandidateDto,
  UpdateCandidateTestScoreDto,
} from '../models/CandidateModels';

@Injectable()
export class CandidateService {
  public API = 'https://localhost:44305/api';
  public CANDIDATE_API = `${this.API}/candidate`;

  constructor(private http: HttpClient) {}

  //#region Candidate
  getCandidate(candidateId: string): Observable<GetCandidateDto> {
    return this.http.get<GetCandidateDto>(
      `${this.CANDIDATE_API}/GetCandidate?candidateId=${candidateId}`
    );
  }

  getCandidatesForCompany(companyId: string): Observable<GetCandidateDto[]> {
    return this.http.get<GetCandidateDto[]>(
      `${this.CANDIDATE_API}/GetCandidatesForCompany?companyId=${companyId}`
    );
  }

  getCandidatesForJobOpening(
    jobOpeningId: string
  ): Observable<GetCandidateDto[]> {
    return this.http.get<GetCandidateDto[]>(
      `${this.CANDIDATE_API}/GetCandidatesForJobOpening?jobOpeningId=${jobOpeningId}`
    );
  }

  getCandidatesForUser(userId: string): Observable<GetCandidateDto[]> {
    return this.http.get<GetCandidateDto[]>(
      `${this.CANDIDATE_API}/GetCandidatesForUser?userId=${userId}`
    );
  }

  createCandidate(candidate: CreateCandidateDto): Observable<GetCandidateDto> {
    return this.http.post<GetCandidateDto>(
      `${this.CANDIDATE_API}/CreateCandidate`,
      candidate
    );
  }

  deleteCandidate(candidateId: string) {
    return this.http.delete(
      `${this.CANDIDATE_API}/DeleteCandidate?candidateId=${candidateId}`
    );
  }
  //#endregion

  //#region CandidateTests
  createCandidateTest(
    candidateTest: CreateCandidateTestDto
  ): Observable<CandidateTestDto> {
    return this.http.post<CandidateTestDto>(
      `${this.CANDIDATE_API}/CreateCandidateTest`,
      candidateTest
    );
  }

  getCandidateTestAsCompanyManager(
    candidateId: string,
    testId: string
  ): Observable<CandidateTestDto[]> {
    return this.http.get<CandidateTestDto[]>(
      `${this.CANDIDATE_API}/GetCandidateTestAsCompanyManager?candidateId=${candidateId}&testId=${testId}`
    );
  }

  getCandidateTestAsCandidate(
    candidateId: string,
    testId: string
  ): Observable<CandidateTestDto[]> {
    return this.http.get<CandidateTestDto[]>(
      `${this.CANDIDATE_API}/GetCandidateTestAsCandidate?candidateId=${candidateId}&testId=${testId}`
    );
  }

  updateCandidateTestScore(
    candidateTestScore: UpdateCandidateTestScoreDto
  ): Observable<any> {
    return this.http.post<any>(
      `${this.CANDIDATE_API}/UpdateCandidateTestScore`,
      candidateTestScore
    );
  }
  //#endregion
}
