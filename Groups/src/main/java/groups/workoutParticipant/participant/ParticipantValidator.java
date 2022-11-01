package groups.workoutParticipant.participant;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface ParticipantValidator {

    HttpStatus checkGympassId(UUID gympassId);
}
