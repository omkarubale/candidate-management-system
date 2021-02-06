import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { CreateTestDto, CreateTestScoreDto, GetTestDto, TestScoreDto, UpdateTestDto, UpdateTestScoreDto } from '../../models/TestModels';

@Injectable()
export class ManagerTestService {
  public API = 'https://localhost:44305/api';
  public TEST_API = `${this.API}/managerTest`;

  constructor(
    private http: HttpClient
  ) { }

  //#region Test
  getTestsAsCompanyManager(): Observable<GetTestDto[]> {
    return this.http.get<GetTestDto[]>(`${this.TEST_API}/GetTestsAsCompanyManager`);
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
    return this.http.delete(`${this.TEST_API}/DeleteTest?testId=${testId}`);
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
