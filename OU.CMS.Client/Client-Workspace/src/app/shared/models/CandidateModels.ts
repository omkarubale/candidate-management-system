import { CreatedOnDto } from "./CommonModels";
import { UserSimpleDto } from "./UserModels";
import { JobOpeningSimpleDto } from "./JobOpeningModels";
import { CompanySimpleDto } from "./CompanyModels";

export class CandidateTestDto {
  Title: string;
  Candidate: GetCandidateDto;
  CandidateTestScores: CandidateTestScoreDto[];
  TotalScore: number;
  Percentile: number;
}

export class CandidateTestsContainerDto {
  Candidate: GetCandidateDto;
  CandidateTests: CandidateTestDto[];
}

export class CandidateTestScoreDto {
  CandidateTestScoreId: string;
  TestScoreId: string;
  Title: string;
  IsMandatory: boolean;
  Value: number;
  MaximumScore: number;
  CutoffScore: number;
  Comment: string;
}

export class CreateCandidateDto {
  UserId: string;
  CompanyId: string;
  JobOpeningId: string;
}

export class CreateCandidateTestDto {
  CandidateId: string;
  TestId: string;
}

export class GetCandidateDto {
  CandidateId: string;
  User: UserSimpleDto;
  Company: CompanySimpleDto;
  JobOpening: JobOpeningSimpleDto;
  CreatedDetails: CreatedOnDto;
}

export class UpdateCandidateTestScoreDto {
  CandidateTestScoreId: string;
  TestScoreTitle: string;
  Value: number;
  Comment: string;
}
