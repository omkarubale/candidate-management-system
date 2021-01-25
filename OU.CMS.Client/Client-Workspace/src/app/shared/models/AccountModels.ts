export class SignInDto {
  Email: string;
  Password: string;
  UserType: number;
}

export class SignUpDto {
  Email: string;
  Password: string;
  ConfirmPassword: string;
  FirstName: string;
  LastName: string;
  IsCandidateLogin: boolean;
  CompanyName: string;
}

export class SignUpByManagerInviteDto {
  Email: string;
  Password: string;
  ConfirmPassword: string;
  FirstName: string;
  LastName: string;
  CompanyId: string;
}

export class RegisterByInvitePageDto {
  Email: string;
  CompanyId: string;
  CompanyName: string;
  InvitedByUserName: string;
}

export class UpdatePasswordDto {
  Password: string;
  ConfirmPassword: string;
}
