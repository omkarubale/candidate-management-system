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
  FullName: string;
  ShortName: string;
  UserType: number;
  IsCandidateLogin: boolean;
  CompanyName: string;
  IsPasswordMatch: boolean;
}
