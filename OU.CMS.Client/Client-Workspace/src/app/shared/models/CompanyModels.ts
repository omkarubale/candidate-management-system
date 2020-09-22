import { CreatedOnDto } from "./CommonModels";

export class AcceptCompanyManagementInviteDto {
  CompanyId: string;
}

export class CompanySimpleDto {
  Id: string;
  Name: string;
}

export class CreateCompanyManagementInviteDto {
  CompanyId: string;
  Email: string;
  IsInviteForAdmin: boolean;
}

export class DeleteCompanyManagementDto {
  CompanyId: string;
  UserId: string;
}

export class GetCompanyDto {
  Id: string;
  Name: string;
  CreatedDetails: CreatedOnDto;
}

export class RevokeCompanyManagementInviteDto {
  CompanyId: string;
  Email: string;
}

export class SaveCompanyDto {
  Id: string;
  Name: string;
}
