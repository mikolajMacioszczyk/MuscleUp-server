package groups.group.controller.form;

import java.time.LocalDateTime;

public record GroupFullForm(
        String name,
        String description,
        LocalDateTime startTime,
        LocalDateTime endTime,
        Boolean repeatable
) { }
