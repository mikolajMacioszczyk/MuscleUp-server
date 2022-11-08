package groups.schedule.controller.form;

import groups.schedule.dto.ScheduleCell;

import java.time.ZonedDateTime;
import java.util.Objects;
import java.util.UUID;

public record ScheduleCellForm(
        String name,
        String description,
        ZonedDateTime startTime,
        ZonedDateTime endTime,
        boolean repeatable,
        String location,
        int maxParticipants,
        UUID workoutId,
        UUID trainerId,
        UUID fitnessClubId
) {

    public boolean hasGroupChanged(ScheduleCell cell) {

        return !Objects.equals(cell.getGroup().name(), name)
                || !Objects.equals(cell.getGroup().description(), description)
                || !Objects.equals(cell.getGroup().location(), location)
                || cell.getGroup().maxParticipants() != maxParticipants
                || !cell.getTrainer().trainerId().equals(trainerId)
                || !cell.getGroup().fitnessClub().equals(fitnessClubId)
                || cell.getGroup().repeatable() != repeatable;
    }

    public boolean hasGroupWorkoutChanged(ScheduleCell cell) {

        return !cell.getWorkout().workoutId().equals(workoutId)
                || !cell.getWorkout().startTime().isEqual(startTime)
                || !cell.getWorkout().endTime().isEqual(endTime);
    }
}
