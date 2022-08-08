export class MessageDetailResponse {
  'usernameFrom':	string;
  'usernameTo':	string;
  'subject':	string;
  'body':	string;
  'timeSent':	Date;
  'receiverRead':	Date | null;
  'replyForId': number | null | undefined;
  'replyForSubject':	string | null | undefined;
}
