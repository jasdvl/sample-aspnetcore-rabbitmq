import { BookDto } from "./book.dto";

export interface BookReservationDto
{
    /**
     * Unique Identifier of the user who reserved the book.
     */
    userId: number;

    book: BookDto;
}
