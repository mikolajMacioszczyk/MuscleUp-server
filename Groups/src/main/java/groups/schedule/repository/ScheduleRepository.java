package groups.schedule.repository;

import groups.schedule.dto.ScheduleCellHolder;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface ScheduleRepository {

    ScheduleCellHolder getById(UUID id);

    Optional<ScheduleCellHolder> findById(UUID id);

    List<ScheduleCellHolder> getAll();

    List<ScheduleCellHolder> getWithClonesByCloneId(UUID id);
}
