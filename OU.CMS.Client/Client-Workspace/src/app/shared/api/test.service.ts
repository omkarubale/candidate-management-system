import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { CreateTestDto, CreateTestScoreDto, GetTestDto, TestScoreDto, UpdateTestDto, UpdateTestScoreDto } from '../models/TestModels';

@Injectable()
export class TestService {
  public API = 'https://localhost:44305/api';
  public TEST_API = `${this.API}/test`;

  constructor(
    private http: HttpClient
  ) { }

  //#region Test
  getAllTestsAsCandidate(): Observable<GetTestDto[]> {
    return this.http.get<GetTestDto[]>(`${this.TEST_API}/GetAllTestsAsCandidate`);
  }

  getAllTestsAsCompanyManager(): Observable<GetTestDto[]> {
    return this.http.get<GetTestDto[]>(`${this.TEST_API}/GetAllTestsAsCompanyManager`);
  }

  getTestAsCandidate(testId: string): Observable<GetTestDto> {
    return this.http.get<GetTestDto>(`${this.TEST_API}/GetTestAsCandidate?testId=${testId}`);
  }

  getTestAsCompanyManager(testId: string): Observable<GetTestDto> {
    return this.http.get<GetTestDto>(`${this.TEST_API}/GetTestAsCompanyManager?testId=${testId}`);
  }

  createTest(test: CreateTestDto): Observable<GetTestDto> {
    return this.http.post<GetTestDto>(`${this.TEST_API}/CreateTest`, test);
  }

  updateTest(test: UpdateTestDto): Observable<GetTestDto> {
    return this.http.post<GetTestDto>(`${this.TEST_API}/UpdateTest`, test);
  }

  deleteTest(testId: string) {
    return this.http.delete(`${this.TEST_API}/UpdateTest?testId=${testId}`);
  }
  //#endregion

  //#region TestScores
  createTestScore(test: CreateTestScoreDto): Observable<TestScoreDto> {
    return this.http.post<TestScoreDto>(`${this.TEST_API}/CreateTestScore`, test);
  }

  updateTestScore(test: UpdateTestScoreDto): Observable<TestScoreDto> {
    return this.http.post<TestScoreDto>(`${this.TEST_API}/UpdateTestScore`, test);
  }

  deleteTestScore(testScoreId: string) {
    return this.http.delete(`${this.TEST_API}/DeleetTestScore?testScoreId=${testScoreId}`);
  }
  //#endregion
}
