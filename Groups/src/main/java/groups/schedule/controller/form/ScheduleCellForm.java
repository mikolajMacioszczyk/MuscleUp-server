package groups.schedule.controller.form;

import groups.schedule.dto.ScheduleCell;

import java.time.LocalDateTime;
import java.util.Objects;
import java.util.UUID;

public record ScheduleCellForm(
        String name,
        String description,
        LocalDateTime startTime,
        LocalDateTime endTime,
        boolean repeatable,
        String location,
        int maxParticipants,
        UUID workoutId,
        UUID trainerId
) {

    public boolean hasGroupChanged(ScheduleCell cell) {

        return !Objects.equals(cell.getGroup().name(), name)
                || !Objects.equals(cell.getGroup().description(), description)
                || hasTrainerChanged(cell)
                || cell.getGroup().repeatable() != repeatable;
    }

    public boolean hasGroupWorkoutChanged(ScheduleCell cell) {

        return !cell.getWorkout().workoutId().equals(workoutId)
                || !Objects.equals(cell.getWorkout().location(), location)
                || cell.getWorkout().maxParticipants() != maxParticipants
                || !cell.getWorkout().startTime().isEqual(startTime)
                || !cell.getWorkout().endTime().isEqual(endTime);
    }

    public boolean hasTrainerChanged(ScheduleCell cell) {

        return !cell.getTrainer().trainerId().equals(trainerId);
    }
}
