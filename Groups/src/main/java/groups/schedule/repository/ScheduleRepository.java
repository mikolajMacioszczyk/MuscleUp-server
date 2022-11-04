package groups.schedule.repository;

import groups.schedule.dto.ScheduleCellHolder;

import java.util.List;
import java.util.UUID;

public interface ScheduleRepository {

    ScheduleCellHolder getById(UUID id);

    List<ScheduleCellHolder> getAll();
}
