package groups.workoutParticipant.participant;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface ParticipantQuery {

    HttpStatus checkUserId(UUID userId, UUID fitnessClubId);
}
