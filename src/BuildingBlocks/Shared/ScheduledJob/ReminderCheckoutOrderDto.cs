namespace Shared.ScheduledJob;

public record ReminderCheckoutOrderDto(string Email, string Subject, string EmailContent, DateTimeOffset EnqueueAt);