import { CreatedOnDto } from "./CommonModels";
import { UserSimpleDto } from "./UserModels";

export class CreateTestDto {
  Title: string;
  Description: string;
}

export class CreateTestScoreDto {
  TestId: string;
  Title: string;
  IsMandatory: boolean;
  MinimumScore: number;
  MaximumScore: number;
  CutoffScore: number;
}

export class GetTestDto {
  Id: string;
  Title: string;
  Description: string;
  CompanyId: string;
  TestScores: TestScoreDto[];
  CreatedDetails: CreatedOnDto;
  TakersCount: number;
}

export class TestScoreDto {
  Id: string;
  Title: string;
  IsMandatory: boolean;
  MinimumScore: number;
  MaximumScore: number;
  CutoffScore: number;
}

export class UpdateTestDto {
  Id: string;
  Title: string;
  Description: string;
}

export class UpdateTestScoreDto {
  Id: string;
  TestId: string;
  Title: string;
  IsMandatory: boolean;
  MinimumScore: number;
  MaximumScore: number;
  CutoffScore: number;
}
