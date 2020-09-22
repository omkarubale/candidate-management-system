import { CreatedOnDto } from "./CommonModels";
import { UserSimpleDto } from "./UserModels";
import { JobOpeningSimpleDto } from "./JobOpeningModels";
import { CompanySimpleDto } from "./CompanyModels";

export class CandidateTestDto {
  CandidateId: string;
  Title: string;
  CandidateTestScores: CandidateTestScoreDto[];
}

export class CandidateTestScoreDto {
  CandidateTestScoreId: string;
  TestScoreId: string;
  Title: string;
  IsMandatory: boolean;
  Value: number;
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
  User: UserSimpleDto;
  Company: CompanySimpleDto;
  JobOpening: JobOpeningSimpleDto;
  CreatedDetails: CreatedOnDto;
}

export class UpdateCandidateTestScoreDto {
  CandidateTestScoreId: string;
  Value: number;
  Comment: string;
}
