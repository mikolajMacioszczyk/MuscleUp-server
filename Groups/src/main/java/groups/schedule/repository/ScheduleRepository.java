package groups.schedule.repository;

import groups.schedule.dto.ScheduleCellHolder;

import java.util.UUID;

public interface ScheduleRepository {

    ScheduleCellHolder getById(UUID id);
}
