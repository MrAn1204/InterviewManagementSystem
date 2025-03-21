import { UserInformation } from "./user-information.model";

export class LoginResponse {
    public accessToken!: string;
    public refreshToken!: string;
    public expiresAt!: Date;
    public userInfo!: UserInformation;
}
