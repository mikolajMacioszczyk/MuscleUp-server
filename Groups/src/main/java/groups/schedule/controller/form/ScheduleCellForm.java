package groups.schedule.controller.form;

import java.time.LocalDateTime;
import java.util.UUID;

public record ScheduleCellForm(
        UUID workoutId,
        UUID trainerId,
        LocalDateTime startTime,
        LocalDateTime endTime,
        String groupName,
        int maxParticipants,
        String location,
        String description
) { }
