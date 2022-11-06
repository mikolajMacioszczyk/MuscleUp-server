package content.common.wrappers;

import content.common.annotation.MustExist;

import java.util.UUID;

@MustExist(reason = MustExist.Reason.HIBERNATE)
public record UuidWrapper(UUID uuid) {
}
