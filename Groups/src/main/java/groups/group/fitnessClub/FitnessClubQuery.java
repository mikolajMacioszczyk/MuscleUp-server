package groups.group.fitnessClub;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface FitnessClubQuery {

    HttpStatus checkFitnessClubId(UUID fitnessClubId);
}
