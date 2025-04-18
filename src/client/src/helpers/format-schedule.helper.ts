import { UserForInputModel } from "../models/data-for-input/user-for-input.model";

export function formatTime(time: string): string {
    const [hours, minutes] = time.split(':');
    return `${hours}:${minutes} ${Number(hours) >= 12 ? 'PM' : 'AM'}`;
}

export function formatName(user: UserForInputModel): string {
    return `${user.fullName} (${user.userName})`;
}

export function formatNameList(users: UserForInputModel[]): string[] {
    return users.map(user => formatName(user));
}