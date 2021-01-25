import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { CreateTestDto, CreateTestScoreDto, GetTestDto, TestScoreDto, UpdateTestDto, UpdateTestScoreDto } from '../../models/TestModels';

@Injectable()
export class CandidateTestService {
  public API = 'https://localhost:44305/api';
  public TEST_API = `${this.API}/candidateTest`;

  constructor(
    private http: HttpClient
  ) { }

  //#region Test
  getTestsAsCandidate(): Observable<GetTestDto[]> {
    return this.http.get<GetTestDto[]>(`${this.TEST_API}/GetTestsAsCandidate`);
  }

  getTestAsCandidate(testId: string): Observable<GetTestDto> {
    return this.http.get<GetTestDto>(`${this.TEST_API}/GetTestAsCandidate?testId=${testId}`);
  }
  //#endregion
}
