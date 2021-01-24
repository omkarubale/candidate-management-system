import { CreatedOnDto } from "./CommonModels";
import { UserSimpleDto } from "./UserModels";

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

export class EditCompanyDto {
  Id: string;
  Name: string;
}

export class CompanyManagerDto {
  User: UserSimpleDto;
  IsAdmin: boolean;
  HasAcceptedInvite: boolean;
  InviteeEmail: string;
}

export class GetCompanyManagementDto {
  CompanyManagers: CompanyManagerDto[];
}
