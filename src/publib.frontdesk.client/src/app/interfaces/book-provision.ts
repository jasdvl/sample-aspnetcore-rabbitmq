import { BookDto } from "./book.dto";

export interface BookProvisionDto
{
    userId: number;

    book: BookDto;

    provisionTimestamp: Date;
}
