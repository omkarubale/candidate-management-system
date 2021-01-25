import { CreatedOnDto } from "./CommonModels";
import { UserSimpleDto } from "./UserModels";
import { CompanySimpleDto } from "./CompanyModels";

export class CreateJobOpeningDto {
  Title: string;
  Description: string;
  Salary: number;
  Deadline: Date;
}

export class GetCandidateJobOpeningDto {
  JobOpeningId: string;
  Title: string;
  Description: string;
  Salary: number;
  Deadline: Date;
  Company: CompanySimpleDto;
  CreatedDetails: CreatedOnDto;

  CandidateId: string;
  AppliedOn: Date;
}

export class GetJobOpeningDto {
  Id: string;
  Title: string;
  Description: string;
  Salary: number;
  Deadline: Date;
  Company: CompanySimpleDto;
  CreatedDetails: CreatedOnDto;
}

export class GetJobOpeningCompanyDto extends GetJobOpeningDto {
  CandidateCount: number;
}

export class JobOpeningSimpleDto {
  Id: string;
  Title: string;
  Description: string;
}

export class UpdateJobOpeningDto {
  Id: string;
  Title: string;
  Description: string;
  Deadline: Date;
  Salary: number;
}
